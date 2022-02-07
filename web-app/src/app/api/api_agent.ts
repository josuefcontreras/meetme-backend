import axios, { AxiosResponse } from "axios";
import { sleep } from "../../utils";
import { Activity } from "../models/Activity";

axios.defaults.baseURL = "https://localhost:5001/api/";

axios.interceptors.response.use(async (response) => {
  await sleep(1000);
  return response;
});

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const request = {
  get: <T>(url: string) => axios.get<T>(url).then(responseBody),
  post: <T>(url: string, body: {}) =>
    axios.post<T>(url, body).then(responseBody),
  put: <T>(url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
  delete: <T>(url: string) => axios.delete<T>(url).then(responseBody),
};

const Activities = {
  create: (activity: Activity) => request.post<void>("/activities", activity),
  delete: (id: string) => request.delete<void>(`/activities/${id}`),
  details: (id: string) => request.get<Activity>(`/activities/${id}`),
  edit: (activity: Activity) =>
    request.put<void>(`/activities/${activity.id}`, activity),
  list: () => request.get<Activity[]>("/activities"),
};

const API_AGENT = {
  Activities,
};

export default API_AGENT;
