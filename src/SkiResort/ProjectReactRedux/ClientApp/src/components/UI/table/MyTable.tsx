import React, { ReactNode } from 'react';
import * as Styles from '../../../styles/components'
import PropTypes, { InferProps } from "prop-types";
import classes from './MyTable.module.css';


const MyTable: any = ({ children, ...props }: InferProps<typeof MyTable.propTypes>) => {
//    return (
//        <div className={classes.parent_block_container} >
//            <div className={classes.block_container}>
//                <div className={classes.block1} style={{ background: "black" }}>
//                    left
//                </div>
//                <div className={classes.block1} style={{ background: "red" }}>
//                    right
//                </div>
//            </div>
//        </div>
//    )
//};

    let actionsAvailable: boolean =  props.headNames[props.headNames.length - 1] != '';
    return (
        <div className={classes.parent_block_container} >
            <div className={classes.block_container}>
                <div className={classes.block1}>

                    <table className={classes.myTable}>
                        <thead>
                            <tr>
                                {props.headNames.slice(0, -1).map((headName: string) =>
                                    <th key={headName} className={classes.myTh}>{headName}</th>
                                )}
                                {actionsAvailable &&
                                    <th key={props.headNames[props.headNames.length - 1]} className={classes.myThLast}>{props.headNames[props.headNames.length - 1]}</th>
                                }
                            </tr>
                        </thead>
                        <tbody>
                            {props.rows.map((row: Array<any>) =>
                                <tr className={classes.MyTr} key={row[0]}>
                                    {row.slice(0, -1).map((data: any) =>
                                        <td key={`r${props.rows.indexOf(row)}c${row.indexOf(data)}`} className={classes.myTd}>{data}</td>
                                    )}
                                    {actionsAvailable &&
                                        <td key={row[row.length - 1]}  className={classes.myTdLast}>{row[row.length - 1]}</td>
                                    }
                                    

                                </tr>
                            )}
                        </tbody>
                    </table>
                    
                </div>
                {/*<div className={classes.block2} style={{ background: "red" }}>*/}
                {/*    {props.rows.map((row: Array<any>) =>*/}
                {/*        <div>*/}
                {/*            {row[row.length - 1]},*/}
                {/*        </div>*/}
                {/*    )}*/}
                {/*</div>*/}
            </div>
        </div>
    )
};





//        <div className={classes.block_container}>
//            <div className={classes.block1}>
//                <table className={classes.myTable}>
//                    <thead>
//                        <tr>
//                            {props.headNames.map((headName: string) =>
//                                <th className={classes.myTh}>{headName}</th>
//                            )}
//                        </tr>
//                    </thead>
//                    <tbody>
//                        {props.rows.slice(0, -2).map((row: Array<any>) =>
//                            <tr className={classes.MyTr} key={row[0]}>
//                                {row.map((data: any) =>
//                                    <td className={classes.myTd}>{data}</td>
//                                )}
//                            </tr>
//                        )}
//                    </tbody>
//                    </table>
//            </div>

//            <div className={classes.block2}>
                
//                {props.rows.map((row: Array<any>) =>
//                    <div>
//                        {row[row.length - 2]},
//                        {row[row.length - 1]},
//                    </div>
//                )}
//            </div>

//           </div>
//        )
//};

MyTable.propTypes = {
    children: PropTypes.oneOfType([
        PropTypes.arrayOf(PropTypes.node),
        PropTypes.node
    ]),

    props: PropTypes.shape(
        {
            onChange: PropTypes.func.isRequired,
            headNames: PropTypes.arrayOf(PropTypes.string),
            rows: PropTypes.arrayOf(PropTypes.arrayOf(PropTypes.any))
        }
    )
};

export default MyTable;