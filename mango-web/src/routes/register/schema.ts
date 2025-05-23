import { z } from 'zod';

export const formSchema = z.object({
	username: z.string().min(2).max(50),
	password: z.string().min(6).max(255),
	token: z.string().min(32).max(32)
});

export type FormSchema = typeof formSchema;
