export interface ITokens {
    accessToken: string | null;
    refreshToken: string | null;
  }
export const accTokenName : string = "accToken";
export const refTokenName : string = "refToken";

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
  localStorage.setItem(accTokenName, tokens.accessToken!);
  localStorage.setItem(refTokenName, tokens.refreshToken!)
  return true;
}
export async function isAuthenticated() : Promise<boolean> {

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
  }
  return true;
}