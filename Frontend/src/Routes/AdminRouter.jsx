import { Route, Routes, Navigate } from "react-router-dom";
import AdminsPage from "../Pages/AdminsPage";
import EmployeesPage from "../Pages/EmployeesPage";
import RootLayout from "../Layouts/RootLayout";

const AdminRouter = () => {
  return (
    <RootLayout>
      <Routes>
        <Route path="/admins" exact element={<AdminsPage />} />
        <Route path="/employees" exact element={<EmployeesPage />} />
        <Route path="*" element={<Navigate to="/employees" />} />
      </Routes>
    </RootLayout>
  );
};

export default AdminRouter;
