<script lang="ts">
    export let questionId : number = 0;
    export let question : string = "Asked question";
    export let userName : string = "Asking user";
    export let title : string = "Title";
    export let viewCount : number = 0;
    export let upvotes : number = 0;
    export let language : string = "Language";
    export let liked : boolean = false;
    import ViewCountSvg from '../assets/ViewCount.svelte';
    import UpvoteSvg from '../assets/Upvote.svelte';
    import { navigate } from 'svelte-routing';

    function handleQuestionSelected(){
        navigate(`/selectedQuestion/${questionId}`);
    }
</script>

<a href="#/" on:click|preventDefault={handleQuestionSelected} class="block max-w-sm p-6 mx-auto mt-8 bg-white border border-gray-200 rounded-lg shadow hover:bg-gray-100 dark:bg-gray-800 dark:border-gray-700 dark:hover:bg-gray-700">
    <p class="font-normal text-sm text-gray-700 dark:text-gray-400">{userName}</p>
    <p class="font-normal text-gray-700 dark:text-gray-400">{language}</p>
    <h5 class="mb-2 text-2xl font-bold tracking-tight text-gray-900 dark:text-white">{title}</h5>
    <p class="font-normal text-gray-700 dark:text-gray-400">{question}</p>
    <div class="inline-flex flex-row items-start font-light text-sm text-gray-700 dark:text-gray-400">
        <ViewCountSvg/>
        {viewCount}
    </div>
    <div class="inline-flex flex-row items-end font-light text-sm text-gray-700 dark:text-gray-400">
        <UpvoteSvg upvoted={liked}/>
        {upvotes}
    </div>
</a>