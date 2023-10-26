import { Button, Popconfirm, Space, Table, Tag } from "antd";
import { useEffect, useState } from "react";
import PropTypes from "prop-types";
import moment from "moment";
import EmployeesService from "../Services/Admin/EmployeesService";
import { UpdateEmployeeModal } from "./Modals";
import AdminsServices from "../Services/Admin/AdminsService";

const TableComponent = ({ isAdmins, data }) => {
  const [columns, setColumns] = useState([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [modalUpdateEmployees, setModalUpdateEmployees] = useState([]);

  const handleTableChange = (pagination) => {
    setCurrentPage(pagination.current);
  };

  const showTotal = (total, range) => {
    return `${range[0]}-${range[1]} de ${total} éléments`;
  };

  useEffect(() => {
    const commonColumnConfig = {
      responsive: ["sm", "md", "lg"],
    };

    const indexColumn = {
      title: "Nr",
      resizable: true,
      width: 5,
      key: "index",
      render: (_, record, index) => {
        const indexNumber = (currentPage - 1) * 6 + index + 1;
        const tagColor = indexNumber % 2 === 0 ? "cyan" : "purple";

        return (
          <div>
            <Tag color={tagColor}>{indexNumber}</Tag>
          </div>
        );
      },
    };

    const actionColumn = {
      title: "Action",
      key: "action",
      render: (_, record, index) => (
        <Space size="middle">
          {!isAdmins && (
            <>
              <Button
                type="primary"
                style={{ backgroundColor: "#f5b700" }}
                onClick={() => {
                  const updatedModalUpdateEmployees = [...modalUpdateEmployees];
                  updatedModalUpdateEmployees[index] = true;
                  setModalUpdateEmployees(updatedModalUpdateEmployees);
                }}
              >
                Edit
              </Button>
              <UpdateEmployeeModal
                initVal={{
                  firstName: record.firstName,
                  lastName: record.lastName,
                  email: record.email,
                  phoneNumber: record.phoneNumber,
                  dateOfBirth: moment(record.dateOfBirth),
                }}
                employeeId={record.id}
                visible={modalUpdateEmployees[index] || false}
                onCancel={() => {
                  const updatedModalUpdateEmployees = [...modalUpdateEmployees];
                  updatedModalUpdateEmployees[index] = false;
                  setModalUpdateEmployees(updatedModalUpdateEmployees);
                }}
              />
            </>
          )}
          <Popconfirm
            title="Delete employee"
            description={
              isAdmins
                ? `Are you sure you want to delete "${record.username}"?`
                : `Are you sure you want to delete "${record.firstName} ${record.lastName}"?`
            }
            onConfirm={() =>
              isAdmins
                ? AdminsServices.deleteAdminById(record.id)
                : EmployeesService.deleteEmployeeById(record.id)
            }
            okText="Yes"
            cancelText="No"
          >
            <Button type="primary" style={{ backgroundColor: "#9d0208" }}>
              Delete
            </Button>
          </Popconfirm>
        </Space>
      ),
    };

    const isAdminColumns = isAdmins
      ? [
          indexColumn,
          {
            title: "Username",
            dataIndex: "username",
            key: "Username",
          },
          actionColumn,
        ]
      : [
          indexColumn,
          {
            title: "First name",
            dataIndex: "firstName",
            key: "firstname",
          },
          {
            title: "Last name",
            dataIndex: "lastName",
            key: "lastname",
          },
          {
            title: "Date of birth",
            dataIndex: "dateOfBirth",
            key: "dateOfBirth",
            ...commonColumnConfig,
            render: (dateOfBirth) => moment(dateOfBirth).format("YYYY-MM-DD"),
          },
          {
            title: "Age",
            dataIndex: "dateOfBirth",
            key: "age",
            ...commonColumnConfig,
            render: (dateOfBirth) => {
              const birthDate = new Date(dateOfBirth);
              const currentDate = new Date();
              const ageInMilliseconds = currentDate - birthDate;
              const ageInYears = Math.floor(
                ageInMilliseconds / (365 * 24 * 60 * 60 * 1000)
              );
              return ageInYears;
            },
          },
          {
            title: "Email",
            dataIndex: "email",
            key: "email",
            ...commonColumnConfig,
          },
          {
            title: "Phone number",
            dataIndex: "phoneNumber",
            key: "phoneNumber",
            ...commonColumnConfig,
          },
          actionColumn,
        ];

    setColumns(isAdminColumns);
  }, [currentPage, isAdmins, modalUpdateEmployees]);

  return (
    <div >
      <Table
        columns={columns}
        onChange={handleTableChange}
        pagination={{
          pageSize: 6,
          showTotal: showTotal,
        }}
        rowKey="id"
        dataSource={data}
      />
    </div>
  );
};

TableComponent.propTypes = {
  isAdmins: PropTypes.bool.isRequired,
  data: PropTypes.array.isRequired,
};

export default TableComponent;
