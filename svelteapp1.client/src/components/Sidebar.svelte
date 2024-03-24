<script lang="ts">
	import { onMount } from "svelte";
	import { type ITokens, accTokenName, refTokenName, isAuthenticated } from "../assets/ITokens";
	export let open : boolean = false;
	let languages : Array<string> = []
	onMount(async () => {
		if (!isAuthenticated){
			return;
		}
		if (!isAuthenticated()){
            return;
        }
        let token: ITokens = {
            accessToken: localStorage.getItem(accTokenName),
            refreshToken: localStorage.getItem(refTokenName)
        }
        const response = await fetch("/fallback/quest/languages/used", {
            method: "GET",
            headers: {
              "Authorization": "Bearer " + token.accessToken
            },
        });
        languages = await response.json();
        console.log('jsonData:', languages);
	});
</script>

<aside class="fixed w-1/4 h-full dark:bg-gray-50 bg-gray-900 border-r-2 shadow-lg" class:open>
	<nav class="p-12 text-xl">
		{#await languages}
			<div>Getting languages list...</div>
		{:then result} 
			{#each languages as language}
				<a class="block font-medium hover:underline text-gray-200 dark:text-gray-600" href="/lang/{language}">{language}</a>
			{/each}
		{:catch}
			<a class="block font-medium hover:underline text-gray-200 dark:text-gray-600" href="/">Home</a>
			<a class="block font-medium hover:underline text-gray-200 dark:text-gray-600" href="/profile">Profile</a>
			<a class="block font-medium hover:underline text-gray-200 dark:text-gray-600" href="/logout">Logout</a>
		{/await}
	</nav>
</aside>

<style>
	aside {
		left: -25%;
		transition: left 0.3s ease-in-out
	}
	
	.open {
		left: 0
	}
</style>