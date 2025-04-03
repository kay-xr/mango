<script lang="ts">
	import { onMount } from 'svelte';
	import { redirect } from '@sveltejs/kit';

	onMount(() => redirect(302, '/login'));
</script>

<main>
	You have been logged out successfully! If you are not automatically redirected, click <a
		href="/login">here</a
	>.
</main>
