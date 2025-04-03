<script lang="ts">
	import '../app.css';

	import * as Sidebar from '$lib/components/ui/sidebar/index.js';
	import AppSidebar from '$lib/components/app-sidebar.svelte';

	const hiddenSidebarRoutes = ['/', '/login', '/error'];

	import { ModeWatcher } from 'mode-watcher';
	import { page } from '$app/stores';
	import { derived } from 'svelte/store';
	import type { LayoutProps } from './$types';

	const showSidebar = derived(page, ($page) => {
		return !hiddenSidebarRoutes.includes($page.url.pathname);
	});

	let { data, children }: LayoutProps = $props();
</script>

<ModeWatcher />
<Sidebar.Provider>
	{#if $showSidebar}
		<AppSidebar username={data.user?.username} />
	{/if}
	<main class="flex w-screen">
		<!--{#if $showSidebar}-->
		<!--	<Sidebar.Trigger class="m-4 p-4"/>-->
		<!--{/if}-->
		{@render children?.()}
	</main>
</Sidebar.Provider>
