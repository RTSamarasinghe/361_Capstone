import { jwtDecode } from "jwt-decode";

export function getToken(): string | null {
    return localStorage.getItem("token");
}

export function getCustomerId(): number | null {
    const token = getToken();
    if (!token) return null;

    try {
        const decoded: any = jwtDecode(token);

        return (
            decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"] || null
        );
    } catch {
        console.log("Something went wrong in auth utils");
        return null;

    }
}