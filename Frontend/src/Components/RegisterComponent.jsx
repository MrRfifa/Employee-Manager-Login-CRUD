import { EyeOutlined, EyeInvisibleOutlined } from "@ant-design/icons";
import { Link, useNavigate } from "react-router-dom";
import { ToastContainer, toast } from "react-toastify";
import AuthService from "../Services/Auth/AuthService";
import registerImage from "../assets/register.jpg";
import { useState } from "react";

function RegisterComponent() {
  const navigate = useNavigate();
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);

  async function onFinish(values) {
    try {
      const response = await AuthService.register(
        values.username,
        values.password,
        values.confirmPassword
      );
      if (response.success) {
        toast.success("Registration successful!");
        setTimeout(() => {
          navigate("/login");
        }, 1500);
      } else {
        toast.error(
          response.error ||
            "Registration failed. Please check your credentials."
        );
      }
    } catch (error) {
      console.error("An error occurred during registration:", error);
      toast.error(
        "An error occurred during registration. Please try again later."
      );
    }
  }

  return (
    <>
      <ToastContainer />
      <div className="grid grid-cols-1 sm:grid-cols-2 h-screen w-full">
        <div className="hidden sm:block">
          <img
            className="w-full h-full object-cover"
            src={registerImage}
            alt="login"
          />
        </div>

        <div className="bg-gray-800 flex flex-col justify-center">
          <form
            className="max-w-[400px] w-full mx-auto bg-gray-900 px-8 p-8 rounded-lg"
            onSubmit={onFinish}
          >
            <h2 className="text-4xl dark:text-white font-bold text-center">
              SIGN UP
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
            <div className="flex flex-col text-gray-400 py-2">
              <label htmlFor="confirmPassword">Confirm Password</label>
              <div className="relative">
                <input
                  className="w-full rounded-lg bg-gray-700 mt-2 p-2 focus:border-blue-500 focus:bg-gray-800 focus:outline-none"
                  type={showConfirmPassword ? "text" : "password"}
                  value={confirmPassword}
                  onChange={(e) => setConfirmPassword(e.target.value)}
                  required
                />
                <button
                  type="button"
                  onClick={() => setShowConfirmPassword(!showPassword)}
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
                Have an account?{" "}
                <Link to="/login" style={{ color: "#1890ff" }}>
                  Sign in here
                </Link>
              </p>
            </div>
            <button
              type="submit"
              className="w-full my-5 py-2 bg-teal-500 shadow-lg shadow-teal-500/50 hover:shadow-teal-500/30 text-white font-semibold rounded-lg"
            >
              Sign up
            </button>
          </form>
        </div>
      </div>
    </>
  );
}

export default RegisterComponent;
