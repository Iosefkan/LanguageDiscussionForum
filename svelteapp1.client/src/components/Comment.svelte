<script lang="ts">
    import { type ITokens, refTokenName, accTokenName, isAuthenticated } from '../assets/ITokens';
    export let commentId : number | null = null;
    export let comment : string = "Asked question";
    export let userName : string = "Asking user";
    export let downvotes : number = 0;
    export let upvotes : number = 0;
    export let liked : boolean = false;
    export let disliked : boolean = false;
    export let fileName : string | null = null;
    export let fileTitle : string | null = null;
    import DownvoteSvg from '../assets/Downvote.svelte';
    import UpvoteSvg from '../assets/Upvote.svelte';
    import AudioPlayer from './AudioPlayer.svelte'
    $: dis = disliked ? "true" : "false";
    $: lik = liked ? "true" : "false";
    $: console.log("dis " + {dis});
    $: console.log("like " + {lik});
    async function Unreact(token : ITokens){
        console.log("unreact");
        const response = await fetch(`/fallback/quest/comment/unreact/${commentId}`, {
            method: "POST",
            headers: {
              "Authorization": "Bearer " + token.accessToken
            },
        });
        let result : number = await response.status;
        if (result == 200){
            liked = false;
            disliked = false;
        }
        console.log(result);
    }
    async function Upvote(){
        console.log("upvote");
        console.log(commentId);
        if (!isAuthenticated){
            return;
        }
        if (commentId == null){
            return;
        }
        let token: ITokens = {
            accessToken: localStorage.getItem(accTokenName),
            refreshToken: localStorage.getItem(refTokenName)
        }
        if (liked == true){
            upvotes -= 1;
            Unreact(token);
            return;
        }
        const response = await fetch(`/fallback/quest/comment/upvote/${commentId}`, {
            method: "POST",
            headers: {
              "Authorization": "Bearer " + token.accessToken
            },
        });
        let result : number = await response.status;
        if (result == 200){
            if (disliked){
                downvotes -= 1;
            }
            upvotes += 1;
            liked = true;
            disliked = false;
        }
        console.log("stopUp");
        console.log(result);
    }
    async function Downvote(){
        console.log("downvote");
        if (!isAuthenticated){
            return;
        }
        if (commentId == null){
            return;
        }
        let token: ITokens = {
            accessToken: localStorage.getItem(accTokenName),
            refreshToken: localStorage.getItem(refTokenName)
        }
        if (disliked == true){
            downvotes -= 1;
            Unreact(token);
            return;
        }
        const response = await fetch(`/fallback/quest/comment/downvote/${commentId}`, {
            method: "POST",
            headers: {
              "Authorization": "Bearer " + token.accessToken
            },
        });
        let result : number = await response.status;
        if (result == 200){
            if (liked){
                upvotes -= 1;
            }
            downvotes += 1;
            disliked = true;
            liked = false;
        }
        console.log("stopDown");
        console.log(result);
    }
</script>

<div class="block max-w-sm p-6 mx-auto my-8 bg-white border border-gray-200 rounded-lg shadow dark:bg-gray-800 dark:border-gray-700">
    <!-- <p class="font-normal text-sm text-gray-700 dark:text-gray-400">{userName}</p> -->
    <h3 class="mb-2 text-2xl font-bold tracking-tight text-gray-900 dark:text-white">{userName}</h3>
    <p class="font-normal mb-3 text-gray-700 dark:text-gray-400">{comment}</p>
    {#if (fileName && fileTitle)}
        <AudioPlayer src={fileName} title={fileTitle} />
    {/if}
    <div class="inline-flex flex-row items-start font-light text-sm text-gray-700 dark:text-gray-400">
        <a href="#/" on:click|preventDefault={Downvote} class="hover:shadow-black dark:hover:shadow-white">
            <DownvoteSvg downvoted={disliked}/>
        </a>
        {downvotes}
        <a href="#/" on:click|preventDefault={Upvote} class="ml-2 hover:shadow-black dark:hover:shadow-white">
            <UpvoteSvg upvoted={liked}/>
        </a>
        {upvotes}
    </div>
</div>