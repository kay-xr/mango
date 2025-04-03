import type { PageServerLoad, Actions } from "./$types.js";
import {fail, redirect} from "@sveltejs/kit";
import {setError, superValidate} from "sveltekit-superforms";
import { zod } from "sveltekit-superforms/adapters";
import { formSchema } from "./schema";
import {db} from "$lib/server/db";
import * as table from "$lib/server/db/schema";
import {eq} from "drizzle-orm";
import {verify} from "@node-rs/argon2";
import * as auth from "$lib/server/auth";

export const load: PageServerLoad = async (event) => {
    if (event.locals.user) {
        return redirect(302, '/dashboard');
    }
    return {
        form: await superValidate(zod(formSchema)),
    };
};

export const actions: Actions = {
    default: async (event) => {
        const form = await superValidate(event, zod(formSchema));
        if (!form.valid) {
            console.error("Invalid form");
            return fail(400, {
                form,
            });
        }

        const username = form.data.username;
        const password = form.data.password;

        const results = await db.select().from(table.user).where(eq(table.user.username, username));
        const existingUser = results.at(0);
        if (!existingUser) {
            setError(form, 'username', 'Wrong username or password');
            return setError(form, 'password', 'Wrong username or password');
        }

        const validPassword = await verify(existingUser.passwordHash, password, {
            memoryCost: 19456,
            timeCost: 2,
            outputLen: 32,
            parallelism: 1
        });
        if (!validPassword) {
            setError(form, 'username', 'Wrong username or password');
            return setError(form, 'password', 'Wrong username or password');
        }

        const sessionToken = auth.generateSessionToken();
        const session = await auth.createSession(sessionToken, existingUser.id);
        auth.setSessionTokenCookie(event, sessionToken, session.expiresAt);

        return redirect(302, '/dashboard');
    },
};