<script lang="ts">
    import { onMount } from 'svelte';
    import QuestionSelected from '../components/QuestionSelected.svelte';
    import Comment from '../components/Comment.svelte';
    import JwtAuthorizeView from '../JwtAuthorizeView.svelte';
    import Navbar from '../components/Navbar.svelte';
    import Sidebar from '../components/Sidebar.svelte';
    import { type ITokens, accTokenName, refTokenName, isAuthenticated } from '../assets/ITokens';
    import ButtonToTheTop from '../components/ButtonToTheTop.svelte';
    import NewComment from '../components/NewComment.svelte';
    export let id : number = 0;
    let open: boolean = false;
    let comments : any = [];
    let creatingComment : boolean = false;

    onMount(async () => {
        if (!isAuthenticated()){
            return;
        }
        let token: ITokens = {
            accessToken: localStorage.getItem(accTokenName),
            refreshToken: localStorage.getItem(refTokenName)
        }
        const response = await fetch(`/fallback/quest/comments/${id}`, {
            method: "GET",
            headers: {
              "Authorization": "Bearer " + token.accessToken
            },
        });
        comments = await response.json();
        console.log('jsonData:', comments);
    });

    function handleReload() {
        location.reload();
    }
    
    function handleCreate() {
        creatingComment = true;
    }
</script>

<JwtAuthorizeView url={`/fallback/quest/${id}`} let:data>
    <main class="min-h-screen bg-gray-50 dark:bg-gray-900">
      <Navbar bind:sidebar={open}/>
      <Sidebar bind:open/>
        <QuestionSelected {...data}/>
        {#if (creatingComment)}
            <NewComment questionId={data.questionId} bind:isCommenting={creatingComment}/>
        {:else}
        <div class="flex flex-col items-center mt-5 -mb-5">
            <button type="button" on:click={handleCreate} class="text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-blue-600 dark:hover:bg-blue-700 focus:outline-none dark:focus:ring-blue-800">Comment</button>
          </div>
        {/if}
        {#await comments}
            <div class="flex flex-col items-center">
                <p class="text-white bg-blue-700 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-blue-600 focus:outline-none">Loading comments...</p>
            </div>
        {:then results}
            {#each comments as comment}
                <Comment {...comment}/>
            {/each}
        {:catch error}
            <div class="flex flex-col items-center">
                <p class="text-sm font-light text-gray-500 dark:text-gray-400">
                    Error while loading comments. <a href="#/" on:click={handleReload} class="font-medium text-primary-600 hover:underline dark:text-primary-500">Click to reload the page.</a>
                </p>
                <!-- <button type="button" class="text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-blue-600 dark:hover:bg-blue-700 focus:outline-none dark:focus:ring-blue-800">Ask a question</button> -->
            </div>
        {/await}

        <!-- {#await commentsGotten()}
        <div class="flex flex-col items-center">
            <p class="text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-blue-600 dark:hover:bg-blue-700 focus:outline-none dark:focus:ring-blue-800">Loading comments...</p>
        </div>
        {:then successful} 
            {#if (successful)}
                {#each comments as comment}
                <Comment {...comment}/>
                {/each}
            {:else}
                <div class="flex flex-col items-center">
                    <p class="text-sm font-light text-gray-500 dark:text-gray-400">
                        Error while loading comments. <a href="#/" on:click={handleReload} class="font-medium text-primary-600 hover:underline dark:text-primary-500">Click to reload the page.</a>
                    </p>
                    <button type="button" class="text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-blue-600 dark:hover:bg-blue-700 focus:outline-none dark:focus:ring-blue-800">Ask a question</button>
                </div>
            {/if}
        {/await} -->
      
      <div></div>
      <!-- <div class="flex flex-col items-center">
        <button type="button" class="text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-blue-600 dark:hover:bg-blue-700 focus:outline-none dark:focus:ring-blue-800">Ask a question</button>
      </div> -->
      <ButtonToTheTop/>
      <div class="mt-8">.</div>
  </main>
  </JwtAuthorizeView>