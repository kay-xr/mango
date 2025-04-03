<script lang="ts">
	import { onMount } from 'svelte';

	onMount(async () => {
		await delay(1000);
	});

	function delay(ms: number) {
		return new Promise((resolve) => setTimeout(resolve, ms));
	}
</script>

<main class="flex w-full h-full">
	<div class="flex w-full h-full justify-center items-center">
		<div class="flex p-4 text-9xl motion-preset-wobble active:motion-preset-confetti">🥭</div>
	</div>
</main>
