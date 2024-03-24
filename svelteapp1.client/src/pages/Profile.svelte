<script lang="ts">
    import { type ITokens, refTokenName, accTokenName, isAuthenticated } from "../assets/ITokens";
    import JwtAuthorizeView from "../JwtAuthorizeView.svelte";
    import Sidebar from "../components/Sidebar.svelte";
    import Navbar from "../components/Navbar.svelte";
    import ButtonToTheTop from "../components/ButtonToTheTop.svelte";
    import NewQuestion from "../components/NewQuestion.svelte";
    import { onMount } from "svelte";
    import Question from "../components/Question.svelte";
    import UserProfile from "../components/UserProfile.svelte";
    import Chart from "../components/Chart.svelte";
    let newQuestion : boolean = false;
    let questions : any = [];
    let open : boolean = false;
    let isAdmin : boolean = false;
    let statistics : any = [];
    let token: ITokens = {
            accessToken: localStorage.getItem(accTokenName),
            refreshToken: localStorage.getItem(refTokenName)
    }
    async function getStatistics(changedAdmin: boolean) {
        console.log(changedAdmin);
        const response = await fetch("/fallback/api/statistics/by_day",{
            method: "GET",
            headers: {
                "Authorization": "Bearer " + token.accessToken,
            },
        });
        let status : number = await response.status;
        if (status == 200){
            statistics = await response.json();
        }
    }

    $: getStatistics(isAdmin);

    async function getUserQuestions() {
        const response = await fetch("/fallback/quest/user",{
            method: "GET",
            headers: {
                "Authorization": "Bearer " + token.accessToken,
            },
        });
        let status : number = await response.status;
        if (status == 200){
            questions = await response.json();
        }
    }
    onMount(async () => {
        getUserQuestions();
    });
    function handleCreate() {
      newQuestion = true;
    }
    function handleReload() {
        location.reload();
    }
    
</script>

<JwtAuthorizeView url={'/fallback/api/user_info'} let:data>
    <main class="min-h-screen h-fit bg-gray-50 dark:bg-gray-900">
      <Navbar bind:sidebar={open}/>
      <Sidebar bind:open/>
      {#if (data)}
        <UserProfile {...data}/>
        {#if (data.isAdmin)}
            <div class="flex flex-col items-center my-6">
                <p class="mt-6 mb-2 text-4xl font-bold tracking-tight text-gray-900 dark:text-white">Chart of number of visits per day</p>
            </div>
            {#await statistics}
                <div class="flex flex-col items-center">
                    <p class="text-white bg-blue-700 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-blue-600 focus:outline-none">Loading chart...</p>
                </div>
            {:then res} 
                <Chart dateLoginCount={statistics}/>
            {/await}
            
        {/if}
        {:else}
            <div class="flex flex-col items-center">
                <p class="text-sm font-light text-gray-500 dark:text-gray-400">
                    Error while loading your profile. <a href="#/" on:click={handleReload} class="font-medium text-primary-600 hover:underline dark:text-primary-500">Click to reload the page.</a>
                </p>
            </div>
        {/if}

        <div class="flex flex-col items-center my-6">
            <p class="mt-6 mb-2 text-4xl font-bold tracking-tight text-gray-900 dark:text-white">Your questions</p>
        </div>

        {#if (newQuestion)}
            <NewQuestion bind:isQuestioning={newQuestion}/>
        {:else}
        <div class="flex flex-col items-center mt-5 -mb-5">
            <button type="button" on:click={handleCreate} class="text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-blue-600 dark:hover:bg-blue-700 focus:outline-none dark:focus:ring-blue-800">Ask a question</button>
        </div>
        {/if}

        {#await questions}
            <div class="flex flex-col items-center">
                <p class="text-white bg-blue-700 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-blue-600 focus:outline-none">Loading your questions...</p>
            </div>
        {:then results}
            {#each questions as question}
                <Question {...question}/>
            {/each}
        {:catch error}
            <div class="flex flex-col items-center">
                <p class="text-sm font-light text-gray-500 dark:text-gray-400">
                    Error while loading your questions. <a href="#/" on:click={handleReload} class="font-medium text-primary-600 hover:underline dark:text-primary-500">Click to reload the page.</a>
                </p>
            </div>
        {/await}
      <ButtonToTheTop/>
      <div class="mt-8">.</div>
  </main>
  </JwtAuthorizeView>