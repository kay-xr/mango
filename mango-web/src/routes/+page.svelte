<script lang="ts">
    import { onMount } from 'svelte';

    onMount(async () => {
        await delay(1000);


    });

    function delay(ms: number) {
        return new Promise( resolve => setTimeout(resolve, ms) );
    }
</script>

<main class="flex flex-col w-screen h-screen justify-center items-center">
    <div class="flex p-4 text-9xl animate-pulse">
        🥭
    </div>
</main>
