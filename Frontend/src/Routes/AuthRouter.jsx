import { Route, Routes, Navigate } from "react-router-dom";
import LoginPage from "../Pages/LoginPage";
import RegisterPage from "../Pages/RegisterPage";

const AuthRouter = () => {
  return (
    <Routes>
      <Route path="/login" exact element={<LoginPage />} />
      <Route path="/signup" exact element={<RegisterPage />} />
      <Route path="*" element={<Navigate to="/login" />} />
    </Routes>
  );
};

export default AuthRouter;
