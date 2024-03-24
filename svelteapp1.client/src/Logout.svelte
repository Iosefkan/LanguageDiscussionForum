<script lang='ts'>
    import JwtLogin from './pages/JwtLogin.svelte';
    const accTokenName : string = "accToken";
    const refTokenName : string = "refToken";
    
    interface ITokens {
      accessToken: string | null;
      refreshToken: string | null;
    }

    async function isLoggedOutCompletely() : Promise<boolean> {
        let token : ITokens = {
        accessToken: localStorage.getItem(accTokenName),
        refreshToken: localStorage.getItem(refTokenName)
        }

        const response = await fetch('/fallback/auth/logout', {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": "Bearer " + token.accessToken
            },
            body: JSON.stringify(token),
        });
        localStorage.removeItem(accTokenName);
        localStorage.removeItem(refTokenName);
        let result = await response.status;
        if (result == 200) {
            console.log('alright logged out')
            return true;
        }
        console.log('Server is weird, man')
        return false;
    }
</script>




{#await isLoggedOutCompletely()}
<span>Проверка аутентификации</span>
{:then logged}
    <JwtLogin/>
{/await}