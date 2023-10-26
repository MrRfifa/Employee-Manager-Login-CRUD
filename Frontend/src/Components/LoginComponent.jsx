import { useState } from "react";
import { Link } from "react-router-dom";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import AuthService from "../Services/Auth/AuthService";
import PropTypes from "prop-types";
import { EyeOutlined, EyeInvisibleOutlined } from "@ant-design/icons";
import loginImage from "../assets/login.jpg";

function LoginComponent() {
  const [loading, setLoading] = useState(false);
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [showPassword, setShowPassword] = useState(false);

  async function onFinish(event) {
    event.preventDefault();

    try {
      setLoading(true);
      const response = await AuthService.login(username, password);

      if (response.token !== null && response.success === true) {
        toast.success("Login successful!");
        window.location.reload("/");
      } else {
        toast.error("Login failed. Please check your credentials.");
      }
    } catch (error) {
      console.error("An error occurred during login:", error);
      toast.error("An error occurred during login. Please try again later.");
    } finally {
      setLoading(false);
    }
  }

  return (
    <>
      <ToastContainer />
      <div className="grid grid-cols-1 sm:grid-cols-2 h-screen w-full">
        <div className="hidden sm:block">
          <img
            className="w-full h-full object-cover"
            src={loginImage}
            alt="login"
          />
        </div>

        <div className="bg-gray-800 flex flex-col justify-center">
          <form
            className="max-w-[400px] w-full mx-auto bg-gray-900 px-8 p-8 rounded-lg"
            onSubmit={onFinish}
          >
            <h2 className="text-4xl dark:text-white font-bold text-center">
              SIGN IN
            </h2>
            <div className="flex flex-col text-gray-400 py-2">
              <label htmlFor="username">Username</label>
              <input
                className="rounded-lg bg-gray-700 mt-2 p-2 focus:border-blue-500 focus:bg-gray-800 focus:outline-none"
                type="text"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
              />
            </div>
            <div className="flex flex-col text-gray-400 py-2">
              <label htmlFor="password">Password</label>
              <div className="relative">
                <input
                  className="w-full rounded-lg bg-gray-700 mt-2 p-2 focus:border-blue-500 focus:bg-gray-800 focus:outline-none"
                  type={showPassword ? "text" : "password"}
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                />
                <button
                  type="button"
                  onClick={() => setShowPassword(!showPassword)}
                >
                  {showPassword ? (
                    <EyeInvisibleOutlined className="text-gray-950 absolute top-5 right-2" />
                  ) : (
                    <EyeOutlined className="text-gray-950 absolute top-5 right-2" />
                  )}
                </button>
              </div>
            </div>
            <div className="flex justify-between text-gray-400 py-2">
              <p>
                Don&apos;t have an account?{" "}
                <Link to="/signup" style={{ color: "#1890ff" }}>
                  Sign up here
                </Link>
              </p>
            </div>
            <button
              type="submit"
              disabled={loading}
              className="w-full my-5 py-2 bg-teal-500 shadow-lg shadow-teal-500/50 hover:shadow-teal-500/30 text-white font-semibold rounded-lg"
            >
              {loading ? "Signing in..." : "Sign in"}
            </button>
          </form>
        </div>
      </div>
    </>
  );
}

LoginComponent.propTypes = {
  loading: PropTypes.bool.isRequired,
};

export default LoginComponent;
