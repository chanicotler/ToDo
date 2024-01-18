import axios from 'axios';

// Set default API address using Config Defaults
axios.defaults.baseURL = "process.env.REACT_APP_API_ADDRESS";

// Add an error interceptor
axios.interceptors.response.use(
  (response) => response,
  (error) => {
    // Log the error to the console or any logging service
    console.error('Axios Error Interceptor:', error);

    // You can also throw the error again to propagate it to the calling code
    return Promise.reject(error);
  }
);

export default {
  getTasks: async () => {
    try {
      const result = await axios.get("/todoitems");
      return result.data;
    } catch (error) {
      console.error('Error fetching tasks:', error);
      throw error;
    }
  },

  addTask: async (name) => {
    try {
      const result = await axios.post("/todoitems", { Name: name, IsComplete: false });
      return result.data;
    } catch (error) {
      console.error('Error adding task:', error);
      throw error;
    }
  },

  setCompleted: async (id, isComplete) => {
    try {
      const result = await axios.put(`/todoitems/${id}`, { IsComplete: isComplete });
      return result.data;
    } catch (error) {
      console.error('Error setting completion status:', error);
      throw error;
    }
  },

  deleteTask: async (id) => {
    try {
      const result = await axios.delete(`/todoitems/${id}`);
      return result.data;
    } catch (error) {
      console.error('Error deleting task:', error);
      throw error;
    }
  }
};
