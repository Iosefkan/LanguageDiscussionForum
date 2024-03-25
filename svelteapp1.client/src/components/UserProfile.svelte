<script lang="ts">
    export let questionCount : number = 0;
    export let upvotes : number = 0;
    export let commentCount : number = 0;
    export let name : string = "Name";
    export let email : string = "Email";
    export const isAdmin : boolean = false;
</script>

<div class="p-16">
    <div class="p-8 bg-gray-50 dark:bg-gray-900 mt-24">
      <div class="grid grid-cols-1">
        <div class="grid grid-cols-3 text-center order-last mt-5">
          <div>
            <p class="font-bold text-gray-700 dark:text-gray-100 text-xl">{questionCount}</p>
            <p class="text-gray-400">Questions</p>
          </div>
          <div>
            <p class="font-bold text-gray-700 dark:text-gray-100 text-xl">{upvotes}</p>
            <p class="text-gray-400">Upvotes</p>
          </div>
          <div>
            <p class="font-bold text-gray-700 dark:text-gray-100 text-xl">{commentCount}</p>
            <p class="text-gray-400">Comments</p>
          </div>
        </div>
        <div class="relative">
          <div class="w-36 h-36 bg-indigo-100 mx-auto rounded-full shadow-2xl dark:shadow-slate-700 absolute inset-x-0 top-0 -mt-24 flex items-center justify-center text-indigo-500 dark:bg-indigo-700">
          <svg xmlns="http://www.w3.org/2000/svg" class="h-24 w-24 dark:fill-indigo-200" viewBox="0 0 20 20" fill="currentColor">
            <path fill-rule="evenodd" d="M10 9a3 3 0 100-6 3 3 0 000 6zm-7 9a7 7 0 1114 0H3z" clip-rule="evenodd" />
          </svg>
          </div>
        </div>
        <div class="space-x-8 flex justify-between mt-20">
        </div>
      </div>
    
      <div class="mt-10 text-center border-b pb-12">
        <h1 class="text-4xl font-medium text-gray-700 dark:text-gray-100">{name}</h1>
        <p class="font-light text-gray-600 mt-3 dark:text-gray-200">{email}</p>
      </div>
    </div>
    </div>