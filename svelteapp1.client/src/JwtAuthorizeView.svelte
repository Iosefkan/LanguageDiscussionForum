<script lang='ts'>
    import JwtLogin from './pages/JwtLogin.svelte';
    import { type ITokens, refTokenName, accTokenName } from './assets/ITokens';
    export let url : string = '/fallback';

    let data : any;

    function isTokenExpired(token: string) : boolean {
      const base64Url = token.split(".")[1];
      const base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/");
      const jsonPayload = decodeURIComponent(
        atob(base64)
          .split("")
          .map(function (c) {
            return "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2);
          })
          .join("")
      );
      const { exp } = JSON.parse(jsonPayload);
      const expired : boolean = Date.now() >= exp * 1000
      return expired
    }

    async function isNewTokensRecieved(tokenInfo : ITokens) : Promise<boolean> {
      let tokensString : string = JSON.stringify(tokenInfo);
      console.log(tokensString);
      const response = await fetch("/fallback/auth/refresh", {
            method: "POST",
            headers: {
              "Accept": "application/json",
              "Content-Type": "application/json"
            },
            body: tokensString
        });
      let status : number  = response.status;
      
      if (status != 200) {
          return false;    
      }
      let tokens : ITokens = await response.json();
      if (!tokens.accessToken || !tokens.refreshToken){
        return false;
      }
      localStorage.setItem(accTokenName, tokens.accessToken);
      localStorage.setItem(refTokenName, tokens.refreshToken)
      return true;
    }
    async function isAuthenticated() : Promise<boolean> {

      let token : ITokens = {
        accessToken: localStorage.getItem(accTokenName),
        refreshToken: localStorage.getItem(refTokenName)
      }
   
      if (!token.accessToken || !token.refreshToken) {
        console.log('no tokens');
        return false;    
      }

      if (isTokenExpired(token.accessToken)) {
        if (!isNewTokensRecieved(token)){
          console.log('no new tokens');
          return false;
        }
        console.log('new tokens')
        token = {
          accessToken: localStorage.getItem(accTokenName),
          refreshToken: localStorage.getItem(refTokenName)
        }
      }
      
      const response = await fetch(url, {
            method: "GET",
            headers: {
              "Accept": "application/json",
              "Authorization": "Bearer " + token.accessToken
            },
        });
      let status : number  = response.status;
      
      if (status != 200) {
          return false;    
      }
      data = await response.json();
      console.log(data);
      console.log('fine');
      return true;
    }
</script>


{#await isAuthenticated()}
<div class="min-h-screen h-fit bg-gray-50 dark:bg-gray-900">
  <div class="flex flex-col items-center my-6">
    <p class="mt-6 mb-2 text-4xl font-bold tracking-tight text-gray-900 dark:text-white">Checking authentication...</p>
  </div>
</div>
{:then authenticated}
  {#if (authenticated) }
  <slot {data}></slot>
  {:else}
    <JwtLogin/>
  {/if}
{/await}