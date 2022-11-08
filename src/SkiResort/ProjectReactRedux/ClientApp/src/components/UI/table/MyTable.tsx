import React, { ReactNode } from 'react';
import * as Styles from '../../../styles/components'
import PropTypes, { InferProps } from "prop-types";
import classes from './MyTable.module.css';


const MyTable: any = ({ children, ...props }: InferProps<typeof MyTable.propTypes>) => {
    return (
        //<table className='table table-striped' aria-labelledby="tabelLabel">
        <table className={classes.myTable}>
            <thead>
                <tr>
                    {props.headNames.map((headName: string) =>
                        <th className={classes.myTh}>{headName}</th>
                    )}
                </tr>
            </thead>
            <tbody>
                {props.rows.map((row: Array<any>) =>
                    <tr className={classes.MyTr} key={row[0]}>
                        {row.map((data: any) =>
                            <td className={classes.myTd}>{data}</td>
                        )}
                    </tr>
                )}
            </tbody>
        </table>
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