import type { Actions, LayoutServerLoad } from './$types';
import { fail, redirect } from '@sveltejs/kit';
import * as auth from '$lib/server/auth';

export const load: LayoutServerLoad = async (event) => {
	return { user: event.locals.user };
};
