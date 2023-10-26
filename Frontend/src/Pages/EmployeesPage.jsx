import { useEffect, useState } from "react"; // Added React import
//import LayoutComponent from "../Components/LayoutComponent";
import TableComponent from "../Components/TableComponent";
import EmployeesService from "../Services/Admin/EmployeesService";
import { LoadingOutlined, FileAddOutlined } from "@ant-design/icons";
import { Alert, Button, Spin } from "antd";
import { AddEmployeeModal } from "../Components/Modals";

export default function EmployeesPage() {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [isError, setIsError] = useState(false);
  const [modalAddEmployee, setModalAddEmployee] = useState(false);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const res = await EmployeesService.fetchEmployeesData();
        setData(res);
        setIsError(false);
      } catch (error) {
        console.error("Error fetching data:", error);
        setIsError(true);
      } finally {
        setIsLoading(false);
      }
    };

    // Fetch data initially
    fetchData();

    // Fetch data every 30 seconds
    const fetchInterval = setInterval(() => {
      fetchData();
    }, 30000);

    return () => {
      // Clear the interval when the component unmounts to prevent memory leaks
      clearInterval(fetchInterval);
    };
  }, []);

  if (isLoading) {
    return (
      <Spin
        indicator={
          <LoadingOutlined
            style={{
              fontSize: 24,
            }}
            spin
          />
        }
      />
    );
  }

  if (isError) {
    return (
      <Alert
        message="Error"
        description="Please retry in a few minutes."
        type="error"
        showIcon
      />
    );
  }

  return (
    <>
      <Button
        size="large"
        type="primary"
        onClick={() => {
          setModalAddEmployee(true);
        }}
        style={{
          float: "right",
          marginRight: "10px",
          backgroundColor: "#00727A",
          color: "white",
          marginBottom: 25,
        }}
        shape="round"
        icon={<FileAddOutlined />}
      >
        Add employee
      </Button>
      <TableComponent isAdmins={false} data={data} />
      <AddEmployeeModal
        visible={modalAddEmployee}
        onCancel={() => {
          setModalAddEmployee(false);
        }}
      />
    </>
  );
}

// export default function EmployeesPage() {
//   return <LayoutComponent currentPage={"1"} mainContent={<Page />} />;
// }
