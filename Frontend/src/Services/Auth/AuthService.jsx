import axios from "axios";

const API_URL = import.meta.env.VITE_REACT_APP_API_URL;

const login = (username, password) =>
  axios
    .post(`${API_URL}Auth/login`, {
      username,
      password,
    })
    .then((response) => {
      if (response.data && response.data.token) {
        const token = "bearer " + response.data.token;
        localStorage.setItem("token", token);
        return { success: true, token };
      } else {
        return { success: false, error: "Invalid response data" };
      }
    })
    .catch((error) => {
      console.error("Login error:", error);
      return { success: false, error: "Login failed" };
    });

const register = (username, password, confirmPassword) =>
  axios
    .post(`${API_URL}Auth/register`, {
      username,
      password,
      confirmPassword,
    })
    .then((response) => {
      if (response.data) {
        return { success: true, message: "Signed up successfully!" };
      } else {
        return { success: false, error: "Invalid response data" };
      }
    })
    .catch((error) => {
      console.error("Registration error:", error);
      return { success: false, error: "Registration failed" };
    });

const AuthService = {
  login,
  register,
};

export default AuthService;
