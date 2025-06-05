const getJsonResponseStartup = async (url: string) => {
    const response = await fetch(url);
    try {
        if (response.ok) {
            const jsonResponse = await response.json();
            return jsonResponse;
        }
    } catch (err) {
        console.log(err);
    }
}

export { getJsonResponseStartup }