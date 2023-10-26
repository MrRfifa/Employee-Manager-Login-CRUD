import axios from "axios";

const API_URL = import.meta.env.VITE_REACT_APP_API_URL;
const token = localStorage.getItem("token");

const fetchAdminsData = async () => {
  const result = await axios.get(`${API_URL}Admin`, {
    headers: {
      Authorization: `${token}`,
    },
  });
  return result.data;
};

const deleteAdminById = async (id) => {
  try {
    const result = await axios.delete(`${API_URL}admin/${id}`, {
      headers: {
        Authorization: token,
      },
    });

    if (result.status === 204) {
      return true;
    } else {
      throw new Error("Failed to delete admin.");
    }
  } catch (error) {
    console.error("Error deleting admin:", error);
    throw new Error("Failed to delete admin.");
  }
};

const AdminsServices = {
  fetchAdminsData,
  deleteAdminById,
};

export default AdminsServices;
