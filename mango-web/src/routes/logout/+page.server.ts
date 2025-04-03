import type { Actions } from '../../../.svelte-kit/types/src/routes/demo/lucia/$types';
import { fail, redirect } from '@sveltejs/kit';
import * as auth from '$lib/server/auth';

export const actions: Actions = {
	default: async (event) => {
		console.log('Logging out user');
		if (!event.locals.session) {
			return fail(401);
		}
		await auth.invalidateSession(event.locals.session.id);
		auth.deleteSessionTokenCookie(event);

		return redirect(302, '/login');
	}
};
