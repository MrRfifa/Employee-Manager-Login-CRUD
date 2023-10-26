import { Button, Col, DatePicker, Form, Input, Modal, Row } from "antd";
import { useState } from "react";
import PropTypes from "prop-types";
import moment from "moment";
import dayjs from "dayjs";
import EmployeesService from "../Services/Admin/EmployeesService";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

const formatDatePickerValue = (date) => {
  return date ? moment(date).format("YYYY-MM-DD HH:mm:ss") : "";
};

export const AddEmployeeModal = ({ visible, onCancel }) => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);

  const onChange = (date, dateString) => {
    formatDatePickerValue(dateString);
  };

  const onFinish = async (values) => {
    setLoading(true);
    try {
      setTimeout(async () => {
        const response = await EmployeesService.addEmployee(values);
        if (response !== "Successfully created") {
          toast.error("Error in adding the employee");
        } else if (response === "Successfully created") {
          toast.success("Employee added successfully");
          form.resetFields();
          setLoading(false);
        }
        onCancel();
      }, 1500);
    } catch (error) {
      toast.error("Retry in few minutes");
    }
  };

  return (
    <Modal
      open={visible}
      onCancel={onCancel}
      title={
        <div style={{ textAlign: "center", fontSize: "24px" }}>
          Add an employee
        </div>
      }
      footer={null}
    >
      <ToastContainer />
      <Form
        style={{ marginTop: 25 }}
        layout="vertical"
        form={form}
        autoComplete="off"
        onFinish={onFinish}
        initialValues={{
          firstName: "",
          lastName: "",
          email: "",
          phoneNumber: "",
          dateOfBirth: "",
        }}
      >
        <Row gutter={16}>
          <Col span={12}>
            <Form.Item
              label="Firstname"
              name="firstname"
              rules={[
                { required: true, message: "Firstname is required" },
                { type: "text", message: "Please type firstname" },
              ]}
            >
              <Input placeholder="Jon" />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item
              label="Lastname"
              name="lastname"
              rules={[
                { required: true, message: "Lastname is required" },
                { type: "text", message: "Please type lastname" },
              ]}
            >
              <Input placeholder="Doe" />
            </Form.Item>
          </Col>
        </Row>
        <Form.Item
          label="Email"
          name="email"
          rules={[
            { required: true, message: "Email is required" },
            { type: "email", message: "Please enter a valid email" },
          ]}
        >
          <Input placeholder="jon.doe@jon.doe" />
        </Form.Item>
        <Row gutter={16}>
          <Col span={12}>
            <Form.Item
              label="Phone number"
              name="phoneNumber"
              rules={[
                {
                  required: true,
                  message: "Phone number is required",
                },
                {
                  pattern: /^\d+$/,
                  message: "Phone number should contain only digits",
                },
                {
                  len: 10,
                  message: "Phone number should be exactly 10 digits",
                },
              ]}
            >
              <Input placeholder="1234567890" />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item
              label="Date of birth"
              name="dateOfBirth"
              rules={[
                {
                  required: true,
                  message: "Date of birth is required",
                },
              ]}
            >
              <DatePicker onChange={onChange} style={{ width: "100%" }} />
            </Form.Item>
          </Col>
        </Row>

        <Form.Item>
          <Button
            type="primary"
            loading={loading}
            htmlType="submit"
            style={{
              backgroundColor: "#00A482",
              color: "white",
              width: "100%",
            }}
          >
            {loading ? null : "Add"}
          </Button>
        </Form.Item>
      </Form>
    </Modal>
  );
};

export const UpdateEmployeeModal = ({
  employeeId,
  visible,
  onCancel,
  initVal,
}) => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);

  const onChange = (date, dateString) => {
    formatDatePickerValue(dateString);
  };

  const onFinish = async (values) => {
    setLoading(true);
    try {
      const response = await EmployeesService.updateEmployee(
        employeeId,
        values
      );
      if (response.success === false) {
        toast.error("Error in updating the employee");
      } else if (response.success === true) {
        toast.success("Employee updated successfully");
        onCancel();
      }
    } catch (error) {
      toast.error("Retry in few minutes");
    } finally {
      setLoading(false);
    }
  };
  const initialValues = {
    firstName: initVal.firstName,
    lastName: initVal.lastName,
    email: initVal.email,
    phoneNumber: initVal.phoneNumber,
    dateOfBirth: dayjs(initVal.dateOfBirth),
  };
  return (
    <Modal
      open={visible}
      onCancel={onCancel}
      title={
        <div style={{ textAlign: "center", fontSize: "24px" }}>
          Update the employee
        </div>
      }
      footer={null}
    >
      <ToastContainer />
      <Form
        style={{ marginTop: 25 }}
        layout="vertical"
        form={form}
        autoComplete="off"
        onFinish={onFinish}
        initialValues={initialValues}
      >
        <Row gutter={16}>
          <Col span={12}>
            <Form.Item
              label="Firstname"
              name="firstName"
              rules={[
                { required: true, message: "Firstname is required" },
                { type: "text", message: "Please type firstname" },
              ]}
            >
              <Input placeholder="Jon" />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item
              label="Lastname"
              name="lastName"
              rules={[
                { required: true, message: "Lastname is required" },
                { type: "text", message: "Please type lastname" },
              ]}
            >
              <Input placeholder="Doe" />
            </Form.Item>
          </Col>
        </Row>
        <Form.Item
          label="Email"
          name="email"
          rules={[
            { required: true, message: "Email is required" },
            { type: "email", message: "Please enter a valid email" },
          ]}
        >
          <Input placeholder="jon.doe@jon.doe" />
        </Form.Item>
        <Row gutter={16}>
          <Col span={12}>
            <Form.Item
              label="Phone number"
              name="phoneNumber"
              rules={[
                {
                  required: true,
                  message: "Phone number is required",
                },
                () => ({
                  validator(_, value) {
                    if (!value || value.length === 10) {
                      return Promise.resolve();
                    }
                    return Promise.reject(
                      new Error("Phone number should be 10 digits.")
                    );
                  },
                }),
              ]}
            >
              <Input placeholder="123456789" />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item
              label="Date of birth"
              name="dateOfBirth"
              rules={[
                {
                  required: true,
                  message: "Date of birth is required",
                },
              ]}
            >
              <DatePicker onChange={onChange} style={{ width: "100%" }} />
            </Form.Item>
          </Col>
        </Row>

        <Form.Item>
          <Button
            type="primary"
            loading={loading}
            htmlType="submit"
            style={{
              backgroundColor: "#00A482",
              color: "white",
              width: "100%",
            }}
          >
            {loading ? null : "Update"}
          </Button>
        </Form.Item>
      </Form>
    </Modal>
  );
};

AddEmployeeModal.propTypes = {
  visible: PropTypes.bool.isRequired,
  onCancel: PropTypes.func.isRequired,
};

UpdateEmployeeModal.propTypes = {
  visible: PropTypes.bool.isRequired,
  onCancel: PropTypes.func.isRequired,
  initVal: PropTypes.object.isRequired,
  employeeId: PropTypes.number.isRequired,
};
