import axios from "axios";

const API_URL = import.meta.env.VITE_REACT_APP_API_URL;
const token = localStorage.getItem("token");

const fetchEmployeesData = async () => {
  const result = await axios.get(`${API_URL}employee`, {
    headers: {
      Authorization: `${token}`,
    },
  });
  return result.data;
};

const addEmployee = async (data) => {
  try {
    const result = await axios.post(
      `${API_URL}employee`,
      {
        firstName: data.firstname,
        lastName: data.lastname,
        email: data.email,
        phoneNumber: data.phoneNumber,
        dateOfBirth: data.dateOfBirth,
      },
      {
        headers: {
          Authorization: `${token}`,
        },
      }
    );
    return result.data;
  } catch (error) {
    return error.response.data;
  }
};

const deleteEmployeeById = async (id) => {
  try {
    const result = await axios.delete(`${API_URL}employee/${id}`, {
      headers: {
        Authorization: token,
      },
    });

    if (result.status === 204) {
      return true;
    } else {
      throw new Error("Failed to delete employee.");
    }
  } catch (error) {
    console.error("Error deleting employee:", error);
    throw new Error("Failed to delete employee.");
  }
};

const getEmployeeById = async (id) => {
  const result = await axios.get(`${API_URL}employee/${id}`, {
    headers: {
      Authorization: token,
    },
  });
  return result.data;
};

const updateEmployee = async (id, data) => {
  try {
    const result = await axios.put(
      `${API_URL}employee/${id}`,
      {
        firstName: data.firstName,
        lastName: data.lastName,
        email: data.email,
        phoneNumber: data.phoneNumber,
        dateOfBirth: data.dateOfBirth,
      },
      {
        headers: {
          Authorization: token,
        },
      }
    );

    if (result.status === 204) {
      return { success: true, message: "Employee updated successfully" };
    } else {
      return { success: false, message: "Update failed" };
    }
  } catch (error) {
    return { success: false, message: error.response.data };
  }
};

const EmployeesService = {
  updateEmployee,
  getEmployeeById,
  fetchEmployeesData,
  addEmployee,
  deleteEmployeeById,
};

export default EmployeesService;
