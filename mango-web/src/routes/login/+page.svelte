<script lang="ts">
	import type { PageData } from './$types.js';
	import LoginForm from './login-form.svelte';
	let { data }: { data: PageData } = $props();
</script>

<main class="flex flex-col items-center justify-center min-h-screen w-screen">
	<div class="text-6xl p-6">🥭</div>
	<LoginForm {data} />
</main>
