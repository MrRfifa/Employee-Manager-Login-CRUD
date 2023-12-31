import { useEffect, useState } from "react";
import { useRef } from "react";
import { motion } from "framer-motion";
import icon from "../../assets/employee.png";
// * React icons
import {
  AppstoreOutlined,
  UserOutlined,
  LeftOutlined,
  MenuOutlined,
  LogoutOutlined,
} from "@ant-design/icons";

import { useMediaQuery } from "react-responsive";
import { NavLink, useLocation } from "react-router-dom";

const Sidebar = () => {
  let isTabletMid = useMediaQuery({ query: "(max-width: 768px)" });
  const [open, setOpen] = useState(isTabletMid ? false : true);
  const sidebarRef = useRef();
  const { pathname } = useLocation();

  useEffect(() => {
    if (isTabletMid) {
      setOpen(false);
    } else {
      setOpen(true);
    }
  }, [isTabletMid]);

  useEffect(() => {
    isTabletMid && setOpen(false);
  }, [pathname, isTabletMid]);

  const Nav_animation = isTabletMid
    ? {
        open: {
          x: 0,
          width: "16rem",
          transition: {
            damping: 40,
          },
        },
        closed: {
          x: -250,
          width: 0,
          transition: {
            damping: 40,
            delay: 0.15,
          },
        },
      }
    : {
        open: {
          width: "16rem",
          transition: {
            damping: 40,
          },
        },
        closed: {
          width: "3.65rem",
          transition: {
            damping: 40,
          },
        },
      };

  return (
    <div>
      <div
        onClick={() => setOpen(false)}
        className={`md:hidden fixed inset-0 max-h-screen z-[998] bg-black/50 ${
          open ? "block" : "hidden"
        } `}
      ></div>
      <motion.div
        ref={sidebarRef}
        variants={Nav_animation}
        initial={{ x: isTabletMid ? -250 : 0 }}
        animate={open ? "open" : "closed"}
        className=" bg-gray-700 text-gray shadow-xl z-[999] max-w-[16rem]  w-[16rem] 
            overflow-hidden md:relative fixed
         h-screen "
      >
        <div className="flex items-center gap-2.5 font-medium border-b py-3 border-slate-300  mx-3">
          <img src={icon} width={45} alt="" />
          <span className="text-xl whitespace-pre text-white">Employee Manager</span>
        </div>

        <div className="flex flex-col  h-full">
          <ul className="whitespace-pre px-2.5 text-[0.9rem] py-5 flex flex-col gap-1  font-medium overflow-x-hidden scrollbar-thin scrollbar-track-white scrollbar-thumb-slate-100   md:h-[68%] h-[70%]">
            <li>
              <NavLink to={"/employees"} className="link text-white">
                <AppstoreOutlined size={25} className="min-w-max " />
                Employees
              </NavLink>
            </li>
            <li>
              <NavLink to={"/admins"} className="link text-white">
                <UserOutlined size={25} className="min-w-max" />
                Admins
              </NavLink>
            </li>
            <li>
              <NavLink
              to={"/login"}
                onClick={() => {
                  localStorage.clear();
                  window.reload()
                }}
                className="link text-white"
              >
                <LogoutOutlined size={25} className="min-w-max" />
                Logout
              </NavLink>
            </li>
          </ul>
        </div>
        <motion.div
          onClick={() => {
            setOpen(!open);
          }}
          animate={
            open
              ? {
                  x: 0,
                  y: 0,
                  rotate: 0,
                }
              : {
                  x: -10,
                  y: -200,
                  rotate: 180,
                }
          }
          transition={{ duration: 0 }}
          className="absolute w-fit h-fit md:block z-50 hidden right-2 bottom-3 cursor-pointer"
        >
          <LeftOutlined className="text-white" size={25} />
        </motion.div>
      </motion.div>
      <div className="m-3 md:hidden  " onClick={() => setOpen(true)}>
        <MenuOutlined size={25} />
      </div>
    </div>
  );
};

export default Sidebar;
