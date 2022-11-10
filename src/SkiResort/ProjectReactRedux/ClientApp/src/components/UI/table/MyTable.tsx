import React, { ReactNode } from 'react';
import * as Styles from '../../../styles/components'
import PropTypes, { InferProps } from "prop-types";
import classes from './MyTable.module.css';


const MyTable: any = ({ children, ...props }: InferProps<typeof MyTable.propTypes>) => {
    return (
        <div className={classes.block_container}>
            <div className={classes.block1}>
                <table className={classes.myTable}>
                    <thead>
                        <tr>
                            {props.headNames.map((headName: string) =>
                                <th className={classes.myTh}>{headName}</th>
                            )}
                        </tr>
                    </thead>
                    <tbody>
                        {props.rows.slice(0, -2).map((row: Array<any>) =>
                            <tr className={classes.MyTr} key={row[0]}>
                                {row.map((data: any) =>
                                    <td className={classes.myTd}>{data}</td>
                                )}
                            </tr>
                        )}
                    </tbody>
                    </table>
            </div>

            <div className={classes.block2}>
                
                {props.rows.map((row: Array<any>) =>
                    <div>
                        {row[row.length - 2]},
                        {row[row.length - 1]},
                    </div>
                )}
            </div>

           </div>
        )
};

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