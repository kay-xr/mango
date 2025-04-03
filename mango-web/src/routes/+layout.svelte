<script lang="ts">
    import '../app.css';

    import * as Sidebar from "$lib/components/ui/sidebar/index.js";
    import AppSidebar from "$lib/components/app-sidebar.svelte";

    const hiddenSidebarRoutes = ['/', '/login', '/error'];

    import { ModeWatcher } from "mode-watcher";
    import { page } from '$app/stores';
    import { derived } from 'svelte/store';

    const showSidebar = derived(page, ($page) => {
        return !hiddenSidebarRoutes.includes($page.url.pathname);
    });

    let { children } = $props();
</script>

<ModeWatcher />
<Sidebar.Provider>
    {#if $showSidebar}
        <AppSidebar />
    {/if}
    <main>
        {#if $showSidebar}
        <Sidebar.Trigger />
        {/if}
        {@render children?.()}
    </main>
</Sidebar.Provider>