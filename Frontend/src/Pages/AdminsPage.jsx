import { useEffect, useState } from "react"; // Added React import
//import LayoutComponent from "../Components/LayoutComponent";
import TableComponent from "../Components/TableComponent";
import { LoadingOutlined } from "@ant-design/icons";
import { Alert, Spin } from "antd";
import AdminsServices from "../Services/Admin/AdminsService";
//import Table from "../Components/Table";

export default function AdminsPage() {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [isError, setIsError] = useState(false);

  useEffect(() => {
    AdminsServices.fetchAdminsData()
      .then((res) => {
        setData(res);
        setIsLoading(false); // Set isLoading to false when data is fetched
      })
      .catch(() => {
        setIsError(true); // Set isError to true if there's an error
        setIsLoading(false); // Set isLoading to false when there's an error
      });
  }, [data]);
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
      <TableComponent isAdmins={true} data={data} />
      {/* <Table /> */}
    </>
  );
}

// export default function AdminsPage() {
//   return <LayoutComponent currentPage={"2"} mainContent={<Page />} />;
// }
