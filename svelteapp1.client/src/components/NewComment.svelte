<script lang="ts">
    import { type ITokens, accTokenName, refTokenName, isAuthenticated } from "../assets/ITokens";
    export let isCommenting : boolean = true;
    export let questionId : number = 0;
    let file: any;
    let text : string = "";
    let title : string = "";
    let valid : boolean = false;
    let error : boolean = false;
    let input: HTMLInputElement;
    function validateForm() {
        if (!text) {
            valid = false;
            return;
        }
        if (file && !title){
            valid = false;
            return;
        }
        valid = true;
    }
    function handleClose() {
        isCommenting = false;
    }
    async function createComment(){
        validateForm();
        if (!valid){
            return;
        }
        if (!isAuthenticated()){
            return;
        }
        let formData = new FormData();
        formData.append("comment", text);
        if (title){
            formData.append("title", title);
            formData.append("audio", file[0])
        }
        
        let token : ITokens = {
          accessToken: localStorage.getItem(accTokenName),
          refreshToken: localStorage.getItem(refTokenName)
        }

        const response = await fetch(`/fallback/quest/comment/create/${questionId}`, {
                method: "POST",
                headers: {
                    //"Content-Type": "multipart/form-data",
                    "Authorization": "Bearer " + token.accessToken
                },
                body: formData,
            });
        let status = await response.status;
        if (status == 200){
            isCommenting == false;
            location.reload();
        }
        else {
            console.log(await response.json());
            error = true;
        }
    }
    function clearFile(){
        file = null;
        input.value = "";
        title = "";
        validateForm();
    }
</script>    



<form on:submit|preventDefault={createComment} class="max-w-5xl mx-auto relative">
    <button type="button" class="absolute right-2 top-2" on:click={handleClose}>
        <svg class="fill-black dark:fill-white" height="10px" width="10px" version="1.1" id="Capa_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" 
            viewBox="0 0 490 490" xml:space="preserve">
        <polygon points="456.851,0 245,212.564 33.149,0 0.708,32.337 212.669,245.004 0.708,457.678 33.149,490 245,277.443 456.851,490 
            489.292,457.678 277.331,245.004 489.292,32.337 "/>
        </svg>
        </button>
    
     <div class="w-full mb-4 border border-gray-200 rounded-lg bg-gray-50 dark:bg-gray-700 dark:border-gray-600">
         <div class="px-4 py-2 bg-white rounded-t-lg dark:bg-gray-800">
             <label for="comment" class="sr-only">Your comment</label>
             <textarea id="comment" bind:value={text} on:input={validateForm} rows="4" class="w-full px-0 text-sm text-gray-900 bg-white border-0 dark:bg-gray-800 focus:ring-0 dark:text-white dark:placeholder-gray-400" placeholder="Write a comment..." required></textarea>
         </div>
         <div class="flex items-center justify-between px-3 py-2 border-t dark:border-gray-600">
            {#if (valid)}
            <button type="submit" class="inline-flex items-center py-2.5 px-4 text-xs font-medium text-center text-white bg-blue-700 rounded-lg focus:ring-4 focus:ring-blue-200 dark:focus:ring-blue-900 hover:bg-blue-800">
                Post comment
            </button>
            {:else}
            <div class="inline-flex items-center py-2.5 px-4 text-xs font-medium text-center text-white bg-gray-500 rounded-lg  ">
                Can't post comment
            </div>
            {/if}
             <div class="flex ps-0 space-x-1 rtl:space-x-reverse sm:ps-2">
                {#if (file)}
                <div>
                    <input type="text" id="title" bind:value={title} on:input={validateForm} class="block w-full p-2 text-gray-900 border border-gray-300 rounded-lg bg-white text-xs focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-800 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500" placeholder="Title for the audio" maxlength="50">
                </div>
                {/if}
                <label for="file">
                 <div class="inline-flex justify-center items-center p-2 text-gray-500 rounded cursor-pointer hover:text-gray-900 hover:bg-gray-100 dark:text-gray-400 dark:hover:text-white dark:hover:bg-gray-600">
                     <svg class="w-4 h-4" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 12 20">
                          <path stroke="currentColor" stroke-linejoin="round" stroke-width="2" d="M1 6v8a5 5 0 1 0 10 0V4.5a3.5 3.5 0 1 0-7 0V13a2 2 0 0 0 4 0V6"/>
                      </svg>
                     <span class="sr-only">Attach file</span>
                 </div>
                </label>
                {#if (file)}
                <button type="button" on:click={clearFile} class="inline-flex justify-center items-center p-2 text-gray-500 rounded cursor-pointer hover:text-gray-900 hover:bg-gray-100 dark:text-gray-400 dark:hover:text-white dark:hover:bg-gray-600">
                    <svg class="w-4 h-4" fill="none" viewBox="0 0 21 21" xmlns="http://www.w3.org/2000/svg">
                        <g fill="none" fill-rule="evenodd" stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" transform="matrix(0 1 1 0 2.5 2.5)">                       
                        <path d="m3.98652376 1.07807068c-2.38377179 1.38514556-3.98652376 3.96636605-3.98652376 6.92192932 0 4.418278 3.581722 8 8 8s8-3.581722 8-8-3.581722-8-8-8"/>            
                        <path d="m4 1v4h-4" transform="matrix(1 0 0 -1 0 6)"/>               
                        </g>
                    </svg>
                </button>
                {/if}
             </div>
             <input type="file" on:change={validateForm} bind:this={input} bind:files={file} id="file" class="hidden" accept="audio/*" capture>
         </div>
     </div>
  </form>
  {#if (error)}
    <div class="flex flex-col items-center">
        <p class="rounded-lg border border-gray-300 bg-white p-2 text-xs text-gray-900 dark:border-gray-600 dark:bg-gray-800 dark:text-white">Error occured while uploading comment, please try later</p>
    </div>
  {/if}
  