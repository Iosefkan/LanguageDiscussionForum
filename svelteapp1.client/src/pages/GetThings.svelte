<script lang="ts">
    let count: number = 0
    $: countDouble = count * 2;
    const increment = () => {
      count += 1
    }
    async function Get() {
        const response = await fetch("/getThings");
        let result = await response;
        let data = result.json()
        return data;
    }
  </script>
  <main>
        <button class="but" on:click={increment}>
        count is {count}, count doubled is {countDouble}
        </button>
        {#await Get()}
            <span>Checking Authentication</span>
        {:then data}
        {#if (data) }
            {#each data as dat}
                <div>
                    {dat.id}
                    {dat.text}
                </div>
            {/each}
        {/if}
      {/await}
  </main>
  
  
  <style>
    .but {
      transition: filter 300ms;
    }
    .but:hover {
      filter: drop-shadow(0 0 2em #646cffaa);
    }
    
  </style>