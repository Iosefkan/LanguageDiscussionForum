<script lang="ts">
    import { navigate } from "svelte-routing";
    import { type ITokens, refTokenName, accTokenName } from "../assets/ITokens";
    let email : string = "";
    let password : string = "";
    let message : string = "";
    let valid : boolean = false;
    let emailReconfirm : boolean = false;
    let emailForReconfirm : string = "";
    let messageReconfirm : string = "";
    function handleRegister() {
        navigate("/register");
    }
    function validateForm() {
        if (!email || !password){
            valid = false;
            return;
        }
        valid = true;
    }
    async function resendEmailChange() {
        emailReconfirm = !emailReconfirm;
    }
    async function submitForm() {
        validateForm();
        if (!valid){
            message = "Fill in all the fields";
            return;
        }
        const data = { email, password };
        let url = "/fallback/auth/login";
        console.log('data sending');
        const response = await fetch(url, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(data),
        });
        const result = await response;
        if (result.ok) {
            let tokens : ITokens = await result.json();
            if (tokens.accessToken && tokens.refreshToken){
                localStorage.setItem(accTokenName, tokens.accessToken);
                localStorage.setItem(refTokenName, tokens.refreshToken)
            }
            window.location.href = "/";
        }
        else message = "Error logging in.";
    }
    async function submitEmail() {
        if (!emailForReconfirm){
            messageReconfirm = "Input email";
            return;
        }
        let url = "/fallback/auth/resend";
        let data = {email : emailForReconfirm}
        const response = await fetch(url, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            },
            body: JSON.stringify(data),
        });
        let json = await response.json();
        messageReconfirm = json.result;
    }
</script>

{#if (!emailReconfirm)}
<section class="bg-gray-50 dark:bg-gray-900">
    <div class="flex flex-col items-center justify-center px-6 py-8 mx-auto md:h-screen lg:py-0">
        <div class="w-full bg-white rounded-lg shadow dark:border md:mt-0 sm:max-w-md xl:p-0 dark:bg-gray-800 dark:border-gray-700">
            <div class="p-6 space-y-4 md:space-y-6 sm:p-8">
                <h1 class="text-xl font-bold leading-tight tracking-tight text-gray-900 md:text-2xl dark:text-white">
                    Log in
                </h1>
                <form class="space-y-4 md:space-y-6" on:submit|preventDefault={submitForm}>
                    <div>
                        <label for="email" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Your email</label>
                        <input type="email" bind:value={email} on:input={validateForm} name="email" id="email" class="bg-gray-50 border border-gray-300 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500" placeholder="name@company.com">
                    </div>
                    <div>
                        <label for="password" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Password</label>
                        <input type="password" bind:value={password} on:input={validateForm} name="password" id="password" placeholder="••••••••" class="bg-gray-50 border border-gray-300 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500">
                    </div>
                    <button type="submit" class="w-full text-gray-900 dark:text-white bg-primary-600 hover:bg-primary-700 focus:ring-4 focus:outline-none focus:ring-primary-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center dark:bg-primary-600 dark:hover:bg-primary-700 dark:focus:ring-primary-800">Login</button>
                    <p class="text-sm font-light text-gray-500 dark:text-gray-400">{message}</p>
                    <p class="text-sm font-light text-gray-500 dark:text-gray-400">
                        Do not have an account? <a href="#/" on:click={handleRegister} class="font-medium text-primary-600 hover:underline dark:text-primary-500">Register here</a>
                    </p>
                    <p class="text-sm font-light text-gray-500 dark:text-gray-400">
                        Did not receive a confirmation letter? <a href="#/" on:click|preventDefault={resendEmailChange} class="font-medium text-primary-600 hover:underline dark:text-primary-500">Clcik here</a>
                    </p>
                </form>
            </div>
        </div>
    </div>
  </section>
  {:else}
  <section class="bg-gray-50 dark:bg-gray-900">
    <div class="flex flex-col items-center justify-center px-6 py-8 mx-auto md:h-screen lg:py-0">
        <div class="w-full bg-white rounded-lg shadow dark:border md:mt-0 sm:max-w-md xl:p-0 dark:bg-gray-800 dark:border-gray-700">
            <div class="p-6 space-y-4 md:space-y-6 sm:p-8">
                <h1 class="text-xl font-bold leading-tight tracking-tight text-gray-900 md:text-2xl dark:text-white">
                    Resend confirmation letter
                </h1>
                <form class="space-y-4 md:space-y-6" on:submit|preventDefault={submitEmail}>
                    <div>
                        <label for="emailRec" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Your email</label>
                        <input type="email" bind:value={emailForReconfirm} name="email" id="emailRec" class="bg-gray-50 border border-gray-300 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500" placeholder="name@company.com">
                    </div>                  
                    <button type="submit" class="w-full text-gray-900 dark:text-white bg-primary-600 hover:bg-primary-700 focus:ring-4 focus:outline-none focus:ring-primary-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center dark:bg-primary-600 dark:hover:bg-primary-700 dark:focus:ring-primary-800">Send email</button>
                </form>
                <p class="text-sm font-light text-gray-500 dark:text-gray-400">{messageReconfirm}</p>
                <div class="flex flex-col items-center">
                    <p class="text-sm font-light text-gray-500 dark:text-gray-400">
                        <a href="#/" on:click|preventDefault={resendEmailChange} class="font-medium text-primary-600 hover:underline dark:text-primary-500">Go back</a>
                    </p>
                </div>
            </div>
        </div>
    </div>
  </section>
  {/if}
