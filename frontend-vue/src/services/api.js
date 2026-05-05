import axios from "axios";

const apiClient = axios.create({
  baseURL: "/api",
  timeout: 5000
});

export async function getHealth() {
  const response = await apiClient.get("/health");
  return response.data;
}
