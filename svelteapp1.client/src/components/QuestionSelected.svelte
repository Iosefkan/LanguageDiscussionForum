<script lang="ts">
    export let questionId : number | null = null;
    export let question : string = "Asked question";
    export let userName : string = "Asking user";
    export let title : string = "Title";
    export let viewCount : number = 0;
    export let upvotes : number = 0;
    export let language : string = "Language";
    export let liked : boolean = false;
    import ViewCountSvg from '../assets/ViewCount.svelte';
    import UpvoteSvg from '../assets/Upvote.svelte';
    import { type ITokens, refTokenName, accTokenName, isAuthenticated } from '../assets/ITokens';

    async function Upvote(){
        if (!isAuthenticated){
            return;
        }
        if (questionId == null){
            return;
        }
        let token: ITokens = {
            accessToken: localStorage.getItem(accTokenName),
            refreshToken: localStorage.getItem(refTokenName)
        }
        const response = await fetch(`/fallback/quest/upvote/${questionId}`, {
            method: "POST",
            headers: {
              "Authorization": "Bearer " + token.accessToken
            },
        });
        let result : number = await response.status;
        if (result == 200){
            liked = !liked;
            if (liked) { 
                upvotes += 1
            }else { 
                upvotes -= 1
            };
        }
        console.log(result);
    }
</script>

<div class="block max-w-5xl p-6 mx-auto my-8 bg-white border border-gray-200 rounded-lg shadow dark:bg-gray-800 dark:border-gray-700">
    <p class="mb-0 font-normal text-xl text-gray-700 dark:text-gray-400">{userName} -> <a href="/lang/{language}" class="font-medium hover:underline">{language}</a></p>
    <!-- <p class="mb-0 font-normal text-2xl text-gray-700 dark:text-gray-400">{language}</p> -->
    <h5 class="mb-4 text-4xl font-bold tracking-tight text-gray-900 dark:text-white">{title}</h5>
    <p class="mb-4 font-normal text-2xl text-gray-700 dark:text-gray-400">{question}</p>
    <div class="inline-flex flex-row items-start font-light text-base text-gray-700 dark:text-gray-400">
        <ViewCountSvg size={"30px"}/>
        {viewCount}
        <a href="#/" on:click|preventDefault={Upvote} class="ml-2 hover:shadow-black dark:hover:shadow-white">
            <UpvoteSvg upvoted={liked} size={"30px"}/>           
        </a>
        {upvotes}
    </div>
</div>