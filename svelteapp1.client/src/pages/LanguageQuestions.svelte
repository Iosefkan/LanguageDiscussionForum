<script lang="ts">
    import JwtAuthorizeView from "../JwtAuthorizeView.svelte";
    import Sidebar from "../components/Sidebar.svelte";
    import Navbar from "../components/Navbar.svelte";
    import Question from "../components/Question.svelte";
    import ButtonToTheTop from "../components/ButtonToTheTop.svelte";
    import NewQuestion from "../components/NewQuestion.svelte";
    export let language : string = "";
    let newQuestion : boolean = false;
    let open: boolean = false;
    function handleCreate() {
      newQuestion = true;
    }
  </script>
  <JwtAuthorizeView url={`/fallback/quest/language/${language}`} let:data>
    <main class="min-h-screen bg-gray-50 dark:bg-gray-900">
      <Navbar bind:sidebar={open}/>
      <Sidebar bind:open/>
      <!-- <h1 class="ml-2 mt-2 mb-4 text-4xl font-extrabold leading-none tracking-tight text-gray-900 md:text-5xl lg:text-6xl dark:text-white">Questions about {language}</h1> -->
      <h5 class="ml-2 mt-2 mb-4 text-4xl font-bold tracking-tight text-gray-900 dark:text-white">Questions about {language}:</h5>
      {#if (newQuestion)}
            <NewQuestion langDefinite={language} bind:isQuestioning={newQuestion}/>
        {:else}
        <div class="flex flex-col items-center mt-5 -mb-5">
            <button type="button" on:click={handleCreate} class="text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-blue-600 dark:hover:bg-blue-700 focus:outline-none dark:focus:ring-blue-800">Ask a question</button>
          </div>
        {/if}
      {#each data as quest}
        <Question {...quest}/>
      {/each}     
      <ButtonToTheTop/>
      <div class="mt-8">.</div>
  </main>
  </JwtAuthorizeView>