import ApplicationPaths from '../../ApplicationPaths'

let isSignedIn = false;


export async function CheckAthentication() {
    var responce = await fetch(ApplicationPaths.isSignedIn);
    var data = await responce.text();
    isSignedIn = data == 'true';
}

export function SetSignIn(value: boolean) {
    isSignedIn = value;
}

export function IsSignedIn() {
    return isSignedIn;
}
