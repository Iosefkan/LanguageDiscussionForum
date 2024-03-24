<script lang="ts">
    import JwtAuthorizeView from "../JwtAuthorizeView.svelte";
    import Sidebar from "../components/Sidebar.svelte";
    import Navbar from "../components/Navbar.svelte";
    import Question from "../components/Question.svelte";
    import ButtonToTheTop from "../components/ButtonToTheTop.svelte";
    import NewQuestion from "../components/NewQuestion.svelte";
    let open: boolean = false;
    let newQuestion : boolean = false;
    function handleCreate() {
      newQuestion = true;
    }
  </script>
  <JwtAuthorizeView url={'/fallback/quest'} let:data>
    <main class="min-h-screen h-fit bg-gray-50 dark:bg-gray-900">
      <Navbar bind:sidebar={open}/>
      <Sidebar bind:open/>
      {#if (newQuestion)}
            <NewQuestion bind:isQuestioning={newQuestion}/>
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
  
  