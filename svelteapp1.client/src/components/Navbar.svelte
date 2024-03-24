<script>
	import Logo from './Logo.svelte'
	import Hamburger from './Hamburger.svelte'
	import Menu from './Menu.svelte'
	
	export let sidebar = false
</script>

<header class="flex justify-between dark:bg-gray-50 bg-gray-900 p-2 items-center text-gray-600 border-b-2 {sidebar ? "sticky top-0 w-full" : ""}">
	<nav class="flex">
		<Hamburger bind:open={sidebar}/>
		<Logo/>	
	</nav>
	
	<Menu/>
</header>