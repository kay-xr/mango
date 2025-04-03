<script lang="ts">
	import { onMount } from 'svelte';
	import { Separator } from '$lib/components/ui/separator';
	import * as Card from '$lib/components/ui/card/index.js';

	let loading: boolean = true;
	let serverHealth: number = 0;

	onMount(async () => {
		loading = true;
		await delay(1000);
		loading = false;
	});

	function delay(ms: number) {
		return new Promise((resolve) => setTimeout(resolve, ms));
	}
</script>

<main class="flex w-full h-full">
	{#if loading}
		<div class="flex w-full h-full justify-center items-center">
			<div class="flex p-4 text-9xl motion-preset-wobble active:motion-preset-confetti">🥭</div>
		</div>
	{:else}
		<div class="motion-preset-focus w-full h-full">
			<div class="text-2xl p-6">Dashboard</div>
			<Separator />
			<div class="grid md:grid-cols-3 p-6 lg:grid-cols-4 gap-2">
				<Card.Root>
					<Card.Header>
						<Card.Title>Hosts Online</Card.Title>
					</Card.Header>
					<Card.Content>
						<div class="flex w-full justify-center text-3xl">1</div>
					</Card.Content>
				</Card.Root>
				<Card.Root>
					<Card.Header>
						<Card.Title>Servers Online</Card.Title>
					</Card.Header>
					<Card.Content>
						<div class="flex w-full justify-center text-3xl">12</div>
					</Card.Content>
				</Card.Root>
				<Card.Root class="lg:col-span-2">
					<Card.Header>
						<Card.Title>Overall Health</Card.Title>
					</Card.Header>
					<Card.Content>
						{#if serverHealth === 0}
							<div class="flex w-full justify-center">
								<p class="hover:motion-preset-confetti text-5xl">✅</p>
							</div>
						{:else if serverHealth === 1}
							<div class="flex w-full justify-center">
								<p class="hover:motion-preset-seesaw text-5xl">⚠️</p>
							</div>
						{:else}
							<div class="flex w-full justify-center">
								<p class="hover:motion-preset-wobble text-5xl">⛔</p>
							</div>
						{/if}
					</Card.Content>
				</Card.Root>
			</div>
		</div>
	{/if}
</main>
