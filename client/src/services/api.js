import axios from "axios";

const apiClient = axios.create({
  baseURL: "https://localhost:8888", // Corrected to https to match gateway configuration
  headers: {
    "Content-Type": "application/json",
  },
});

// Shorten URL (Post request to /createUrl)
export const shortenUrl = async (originalUrl) => {
  try {
    // Make sure the request body is an object with 'originalUrl' key
    const response = await apiClient.post("/createUrl", { originalUrl });
    return response.data;
  } catch (error) {
    console.error("Error shortening URL:", error);
    alert("Error!")
    throw error;
  }
};


// Fetch all URLs (Get request to /getUrl)
export const fetchUrls = async () => {
  try {
    const response = await apiClient.get("/getUrl"); 
    return response.data; 
  } catch (error) {
    console.error("Error fetching URLs:", error);
    throw error;
  }
};

// Delete a URL by ID (Delete request to /deleteUrl/{id})
export const deleteUrl = async (id) => {
  try {
    await apiClient.delete(`/deleteUrl/${id}`); 
  } catch (error) {
    console.error("Error deleting URL:", error);
    throw error;
  }
};

// Get Original Url (Get request to /{shortCode})
export const firstUrl = async (shortCode) => {
  try {
    await apiClient.get(`/${shortCode}`); // Correct path to match gateway
  } catch (error) {
    console.error("Error getting original URL:", error);
    throw error;
  }
};
