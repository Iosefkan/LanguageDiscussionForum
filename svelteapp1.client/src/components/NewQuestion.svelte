<script lang="ts">
    import { type ITokens, accTokenName, refTokenName, isAuthenticated } from "../assets/ITokens";
    import { onMount } from "svelte";
    export let isQuestioning : boolean = true;
    export let langDefinite : string | null = null;
    interface ILanguage {
        id: number;
        lang: string;
    }
    let question : string = "";
    let title : string = "";
    let language : ILanguage;
    let valid : boolean = false;
    let error : boolean = false;
    let languages : Array<ILanguage> = [];
    let defaultLang : ILanguage = {
        id: -24,
        lang: "Choose a language"
    }
    function validateForm() {
        if (langDefinite){
            if (!question || !title){
                valid = false;
                console.log("first");
                return;
            }
            valid = true;
            console.log("second");
            return;
        }
        if (!question || !title || language.id < 0) {
            valid = false;
            console.log("third");
            return;
        }
        console.log("fourth");
        valid = true;
    }
    function handleClose() {
        isQuestioning = false;
    }
    onMount(async () => {
        if (!isAuthenticated()){
            return;
        }
        let token: ITokens = {
            accessToken: localStorage.getItem(accTokenName),
            refreshToken: localStorage.getItem(refTokenName)
        }
        const response = await fetch(`/fallback/quest/languages`, {
            method: "GET",
            headers: {
              "Authorization": "Bearer " + token.accessToken
            },
        });
        languages = await response.json();
        console.log('jsonData:', languages);
    });

    async function createComment(){
        validateForm();
        if (!valid){
            return;
        }
        if (!isAuthenticated()){
            return;
        }
        let formData = new FormData();
        formData.append("question", question);
        formData.append("title", title);
        formData.append("language", langDefinite == null ? language.lang : langDefinite);
        
        let token : ITokens = {
          accessToken: localStorage.getItem(accTokenName),
          refreshToken: localStorage.getItem(refTokenName)
        }

        const response = await fetch("/fallback/quest/create", {
                method: "POST",
                headers: {
                    //"Content-Type": "multipart/form-data",
                    "Authorization": "Bearer " + token.accessToken
                },
                body: formData,
            });
        let status = await response.status;
        if (status == 200){
            isQuestioning == false;
            location.reload();
        }
        else {
            console.log(await response.json());
            error = true;
        }
    }
</script>    

<form on:submit|preventDefault={createComment} class="max-w-5xl mx-auto relative">
    <button type="button" class="absolute right-2 top-2" on:click={handleClose}>
      <svg class="fill-black dark:fill-white" height="10px" width="10px" version="1.1" id="Capa_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" viewBox="0 0 490 490" xml:space="preserve">
        <polygon
          points="456.851,0 245,212.564 33.149,0 0.708,32.337 212.669,245.004 0.708,457.678 33.149,490 245,277.443 456.851,490 
        489.292,457.678 277.331,245.004 489.292,32.337 "
        />
      </svg>
    </button>
    <div class="mb-4 w-full rounded-lg border border-gray-200 bg-gray-50 dark:border-gray-600 dark:bg-gray-700">
        {#if (langDefinite)}
        <div id="countries" class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-t-lg block w-full px-4 py-2 dark:bg-gray-700 dark:border-gray-600 dark:text-white">{langDefinite}</div>
        {:else}
            {#await languages}
            <div id="countries" class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-t-lg block w-full px-4 py-2 dark:bg-gray-700 dark:border-gray-600 dark:text-white">Loading languages...</div>
            {:then result} 
            <select id="countries" bind:value={language} on:change={validateForm} class="bg-gray-200 border mt-6 border-gray-300 text-gray-900 text-sm focus:ring-blue-500 focus:border-blue-500 block w-full px-3 py-2 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500">
                <option value={defaultLang} selected disabled hidden>{defaultLang.lang}</option>
                {#each languages as lang}
                <option value={lang}>
                    {lang.lang}
                </option>
                {/each}
            </select>
            {:catch error}
            <div id="countries" class="bg-gray-200 border border-gray-300 text-gray-900 text-sm rounded-t-lg block w-full px-4 py-2 dark:bg-gray-700 dark:border-gray-600 dark:text-white">Error while loading languages.</div>
            {/await}
        {/if}
        <div class="rounded-t-lg bg-gray-100 px-4 py-2 dark:bg-gray-900">
        <label for="title" class="sr-only">Your title</label>
        <input id="title" bind:value={title} on:input={validateForm} class="w-full bg-gray-100 border-0 px-0 text-base text-gray-900 focus:ring-0 dark:bg-gray-900 dark:text-white dark:placeholder-gray-300" maxlength="100" placeholder="Write a title..." />
      </div>
      <div class="bg-white px-4 py-2 dark:bg-gray-800">
        <label for="question" class="sr-only">Your question</label>
        <textarea id="question" bind:value={question} on:input={validateForm} rows="4" class="w-full border-0 bg-white px-0 text-sm text-gray-900 focus:ring-0 dark:bg-gray-800 dark:text-white dark:placeholder-gray-400" placeholder="Write a comment..." required></textarea>
      </div>
      {#if (valid)}
      <div class="flex items-center justify-between border-t px-3 py-2 dark:border-gray-600">
        <button type="submit" class="inline-flex items-center rounded-lg bg-blue-700 px-4 py-2.5 text-center text-xs font-medium text-white hover:bg-blue-800 focus:ring-4 focus:ring-blue-200 dark:focus:ring-blue-900">Post question</button>
      </div>
        {:else}
        <div class="flex items-center justify-between border-t px-3 py-2 dark:border-gray-600">
            <div class="inline-flex items-center rounded-lg text-white bg-gray-500 px-4 py-2.5 text-center text-xs font-medium ">Can't post question</div>
        </div>
        {/if}
    </div>
  </form>
  {#if (error)}
  <div class="flex flex-col items-center">
    <p class="rounded-lg border border-gray-300 bg-white p-2 text-xs text-gray-900 dark:border-gray-600 dark:bg-gray-800 dark:text-white">Error occured while uploading question, please try later</p>
  </div>
  {/if}