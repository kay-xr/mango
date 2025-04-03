import { pgTable, serial, text, integer, timestamp } from 'drizzle-orm/pg-core';

export const user = pgTable('user', {
	id: text('id').primaryKey(),
	age: integer('age'),
	username: text('username').notNull().unique(),
	passwordHash: text('password_hash').notNull()
});

export const session = pgTable('session', {
	id: text('id').primaryKey(),
	userId: text('user_id')
		.notNull()
		.references(() => user.id),
	expiresAt: timestamp('expires_at', { withTimezone: true, mode: 'date' }).notNull()
});

export const host = pgTable('host', {
	id: text('id').primaryKey(),
	address: text('address').notNull(),
	port: integer('port').notNull(),
	hostname: text('hostname').notNull(),
	lastSeen: timestamp('last_seen', { withTimezone: true, mode: 'date' }),
})

export type Session = typeof session.$inferSelect;

export type User = typeof user.$inferSelect;
