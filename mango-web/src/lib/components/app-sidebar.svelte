<script lang="ts">
	import NavMain from '$lib/components/nav-main.svelte';
	import NavUser from '$lib/components/nav-user.svelte';
	import * as Sidebar from '$lib/components/ui/sidebar/index.js';
	import type { ComponentProps } from 'svelte';

	import { Gauge } from '@lucide/svelte';
	import { Settings2 } from '@lucide/svelte';
	import { useSidebar } from '$lib/components/ui/sidebar/index.js';
	import { Button } from '$lib/components/ui/button';
	const sidebar = useSidebar();

	let {
		username = 'Not logged in!',
		ref = $bindable(null),
		collapsible = 'icon',
		...restProps
	} = $props() as ComponentProps<typeof Sidebar.Root> & { username: string };

	const data = {
		user: {
			name: username
		},
		navMain: [
			{
				title: 'Dashboard',
				url: '/dashboard',
				icon: Gauge
			},
			{
				title: 'Settings',
				url: '/settings',
				icon: Settings2
			}
		]
	};
</script>

<Sidebar.Root bind:ref {collapsible} {...restProps}>
	<Sidebar.Header>
		<Button
			variant="ghost"
			onclick={() => sidebar.toggle()}
			class="flex text-xl w-full items-center justify-center"
		>
			<div class="">🥭</div>
		</Button>
	</Sidebar.Header>
	<Sidebar.Content>
		<NavMain items={data.navMain} />
	</Sidebar.Content>
	<Sidebar.Footer>
		<NavUser user={data.user} />
	</Sidebar.Footer>
	<Sidebar.Rail />
</Sidebar.Root>
