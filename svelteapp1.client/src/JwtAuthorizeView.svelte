<script lang='ts'>
    import Login from './pages/Login.svelte';
    
    export let url : string = '/';
    

    interface ITokens {
      accessToken: string | null;
      refreshToken: string | null;
    }

    let data;
    const accTokenName : string = "accToken";
    const refTokenName : string = "refToken";

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

    async function isNewTokensRecieved() : Promise<boolean> {
      const response = await fetch("/auth/refresh", {
            method: "POST",
            headers: {
              "Accept": "application/json"
            },
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
          return false;    
      }

      if (isTokenExpired(token.accessToken)) {
        if (!isNewTokensRecieved()){
          return false;
        }
        token = {
          accessToken: localStorage.getItem(accTokenName),
          refreshToken: localStorage.getItem(refTokenName)
        }
      }
      
      const response = await fetch(url, {
            method: "POST",
            headers: {
              "Accept": "application/json", "Authorization": "Bearer " + token.accessToken
            },
        });
      let status : number  = response.status;
      
      if (status != 200) {
          return false;    
      }
      let j = await response.json();
      data = j;
      return true;
    }
</script>


{#await isAuthenticated()}
<span>Проверка аутентификации</span>
{:then authenticated}
  {#if (authenticated) }
  <slot></slot>
  {:else}
    <Login/>
  {/if}
{/await}