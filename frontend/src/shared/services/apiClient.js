const API_BASE_URL = import.meta.env.VITE_API_BASE_URL

export async function apiRequest(endpoint, options = {}){
    const token = sessionStorage.getItem('accessToken')

    const response = await fetch(`${API_BASE_URL}${endpoint}`, {...options,
        headers: {
            'Content-Type': 'application/json',

            ...(token && {
                Authorization: `Bearer ${token}`
            }),

            ...options.headers
        }
    })

    const data = await response.json().catch(() => null)

    if(!response.ok){
        const errorMessage = 
            data?.message ||
            data?.title ||
            'Something went wrong. Please try again.'

        throw new Error(errorMessage)
    }

    return data
}