////import React from 'react';
////import MyButton from "./UI/button/MyButton";
////import { useHistory } from 'react-router-dom';

////export default class LiftItem extends React.Component {

////    render() {
////        const router = useHistory()

////        return (
////            <div className="lift">
////                <div className="lift__content">
////                    <strong>{props.lift.liftName}</strong>
////                    <div>
////                        {props.lift.isOpen}
////                    </div>
////                </div>
////                <div className="lift__btns">
////                    <MyButton onClick={() => router.push(`/lifts/${props.lift.liftName}`)}>
////                        Открыть
////                    </MyButton>
////                    <MyButton onClick={() => props.remove(props.lift)}>
////                        Удалить
////                    </MyButton>
////                </div>
////            </div>
////        );
////    }
////};
